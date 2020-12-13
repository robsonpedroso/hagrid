import { AccountService } from './../accounts.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { SelectComponent, OffClickDirective } from 'ng2-select';
import { StoresService } from '../../stores/stores.service';
import { NotificationsService } from 'angular2-notifications';
import { BsModalComponent } from 'ng2-bs3-modal';
import { Url } from 'url';
import { ConfirmationService } from '@jaspero/ng2-confirmations';
import { ResolveEmit } from '@jaspero/ng2-confirmations/src/interfaces/resolve-emit';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';

export enum TypeImport { Internal = "ImportInternalAccounts", External = "ImportExternalAccounts" }

@Component({
    selector: 'app-account-import',
    templateUrl: './account-import.component.html',
    styles: []
})

export class AccountImportComponent implements OnInit {
    @ViewChild('selectStore') selectStore: SelectComponent;
    @ViewChild('viewErrors')
    modal: BsModalComponent;

    typeImportSelected: TypeImport = TypeImport.Internal;
    typeImport = TypeImport;
    store: any = {};
    requisitions: any = {
        list: [],
        internal: {},
        listErrors: []
	}
	currentImportPermission: any = {};

    constructor(private storeService: StoresService,
        private accountsService: AccountService,
        private notify: NotificationsService,
		private notifyConfirm: ConfirmationService,
		private authPermissionService: AuthPermissionService) { }

    ngOnInit() {
		this.selectStore.items = [];
		this.currentImportPermission = this.authPermissionService.getCurrentPermission(Keys.UserModule.Imports);
	}

    close() {
        this.modal.close();
    }

	downloadCsv(code: string) {
		this.accountsService.downloadCsv(code).subscribe(req => {
			window.open(req.toString(), "_blank");
		});
	}

    openViewErrors(items: any[]) {
        this.requisitions.listErrors = items;
        this.modal.open();
    }

    public filter() {

    }

    public getStores(term: any): void {
        if (term && term.length == 2) {
            this.storeService.getByTerm(term).subscribe((response) => {
                this.selectStore.items = response.map(result => {
                    return { id: result.code, text: result.name }
                });
            });
        }
    }

    public setStore(value: any): void {
        this.store = {
            code: value.id,
            name: value.text
        };

        this.getRequisitions();
    }

    public changeTypeImport(value: any) {
        this.typeImportSelected = value;
    }

    public getRequisitions() {
        if (this.store.code) {
            this.accountsService.getRequisitions(this.store.code).subscribe(req => {
                this.requisitions.list = req;
            });
        }
    }

    public cancel(code: string) {
        this.notifyConfirm.create('Atenção!', 'Deseja realmente cancelar essa requisição?')
            .subscribe((ans: ResolveEmit) => {
                if (ans.resolved) {
                    if (code) {
                        this.accountsService.deleteRequisition(code).subscribe(req => {
                            this.getRequisitions();
                        });
                    }
                }
            });
    }

    public fileUpload(event) {
        let fileList: FileList;

        if (event.dataTransfer != null)
            fileList = event.dataTransfer.files;
        else if (event.target != null)
            fileList = event.target.files;

        if (fileList.length > 0) {
            let file: File = fileList[0];

            if (file.type != "text/plain") {
                this.notify.warn("Atenção!", "Arquivo no formato inválido!");
                return;
            }

            let formData: FormData = new FormData();
            formData.append('file', file);
            formData.append('store_code', this.store.code);
            formData.append('type_requisition', TypeImport.External);

            this.accountsService.saveFileRequisition(formData).subscribe(
                response => {
                    this.notify.success("Sucesso!", "Arquivo enviado com sucesso!");
                    this.getRequisitions();
                }
            )
        }
    }

    //#region Drag and Drop Upload Logo
    public onDrop(event: any) {
        event.preventDefault();
        event.stopPropagation();

        this.fileUpload(event);
    }

    public onDragOver(evt) {
        evt.preventDefault();
        evt.stopPropagation();
    }

    public onDragLeave(evt) {
        evt.preventDefault();
        evt.stopPropagation();
    }
    //#endregion

    public saveDBRequisition() {
        this.requisitions.internal.store_code = this.store.code;
        this.requisitions.internal.type_requisition = TypeImport.Internal;

        this.accountsService.saveDBRequisition(this.requisitions.internal).subscribe(
            response => {
                this.notify.success("Sucesso!", "Solicitação enviada com sucesso!");
                this.requisitions.internal = {};
                this.getRequisitions();
            }
        )
    }
}
