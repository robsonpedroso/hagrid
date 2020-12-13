import { Injectable, Injector } from "@angular/core";
import { HttpEvent, HttpHandler, HttpInterceptor, HttpResponse, HttpErrorResponse, HttpHeaders } from "@angular/common/http";
import { Observable } from "rxjs/Observable";
import { HttpRequest } from "@angular/common/http";
import { Router } from "@angular/router";
import { NotificationsService } from "angular2-notifications";
import { AuthService } from "../auth/auth.service";
import { environment } from "../../../environments/environment";
import { SessionStorageService } from "ngx-webstorage";
import 'rxjs/add/operator/mergeMap';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/operator/switchMap';
import 'rxjs/add/operator/finally';
import 'rxjs/add/observable/throw';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {

    private authService: AuthService;

    constructor(
        private injector: Injector,
        private router: Router,
        private notify: NotificationsService,
        private sessionStorageService: SessionStorageService) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {

        let account = this.sessionStorageService.retrieve(AuthService.ACCOUNT_AUTH_KEY);

        if (!account) {

            return next.handle(req.clone({ url: this.fixUrl(req.url) }))
                .map(event => {
                    if (event instanceof HttpResponse && event.body.content) {
                        return event.clone({ body: event.body.content });
                    }
                    return event;
                })
                .catch((res) => {

                    switch (res.status) {
                        case 401:
                        case 403:
                        case 400:
                            this.badRequest(res);
                            break;
                        case 500:
                            this.internalServerError(res);
                    }

                    return Observable.throw(res);
                });
        }
        else {

            const clonedRequest = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${account.token.access_token}`),
                url: this.fixUrl(req.url)
            });

            return next.handle(clonedRequest)
                .map(event => {
                    if (event instanceof HttpResponse && event.body.content) {
                        return event.clone({ body: event.body.content });
                    }
                    return event;
                })
                .catch((res) => {

                    switch (res.status) {
                        case 400:
                            this.badRequest(res);
                            break;
                        case 401:
                            return this.unauthorized(req, res, next, account);
                        case 403:
                            this.forbidden(res);
                            break;
                        case 500:
                            this.internalServerError(res);
                    }

                    return Observable.throw(res);
                });
        }
    }

    private badRequest(res: Response) {

        if (res instanceof HttpErrorResponse) {

            this.authService = this.injector.get(AuthService);

            if (res.error && res.error.messages) {

                if (res.error.messages[0].text.includes('invalid_transfer_token')) {
                    this.authService.redirectLoginPage();
                }
                else if (res.error.messages[0].text.includes('unauthorized')) {
                    this.router.navigate(['forbidden']);
                } else if (res.error.messages[0].text.indexOf('"Message":') > -1){
                  let errorMessage : string = res.error.messages[0].text.substring(12, res.error.messages[0].text.length -2);

                  if(errorMessage == "invalid_password"){
                    errorMessage = "Senha atual inválida";
                  }

                  this.notify.error("Atenção", errorMessage);
                } else {
                    this.notify.error("Atenção", res.error.messages[0].text);
                }
            }
            else {
                this.authService.redirectLoginPage();
            }
        }
    }

    private unauthorized(req: HttpRequest<any>, res: Response, next: HttpHandler, account: any) {

        this.authService = this.injector.get(AuthService);

        return this.authService.refreshToken().flatMap((response: any) => {
            if (response && response.access_token) {
                account.token = {
                    access_token: response.access_token,
                    refresh_token: response.refresh_token
                };
                this.sessionStorageService.store(AuthService.ACCOUNT_AUTH_KEY, account);

            } else {
                this.sessionStorageService.clear(AuthService.ACCOUNT_AUTH_KEY);

                this.authService.redirectLoginPage();

                return Observable.throw(res);
            }

            let clonedRequestRepeat = req.clone({
                headers: req.headers.set('Authorization', `Bearer ${account.token.access_token}`),
                url: this.fixUrl(req.url)
            });

            return next.handle(clonedRequestRepeat).map(event => {
              if (event instanceof HttpResponse && event.body.content) {
                  return event.clone({ body: event.body.content });
              }
              return event;
          });
        })
    }

    private forbidden(res: Response) {
        this.router.navigate(['forbidden'])
    }

    private internalServerError(res: Response) {

        if (res instanceof HttpErrorResponse) {
            this.notify.error("Atenção!", 'Ops! Estamos com problemas no momento tente novamente');
        }
    }

    private fixUrl(url: string) {
        if (url.indexOf('http://') >= 0 || url.indexOf('https://') >= 0) {
            return url;
        } else {
            return  environment.urls.accountEndpoint + url;
        }
    }
}
