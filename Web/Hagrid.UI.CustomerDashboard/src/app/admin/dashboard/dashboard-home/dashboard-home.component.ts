import { AuthService } from '../../../shared/auth/auth.service';
import { CustomerService } from '../../../shared/services/customer.service';
import { SuperpointsService } from '../../../shared/services/superpoints.service';
import { Component, OnInit } from '@angular/core';
import { SessionStorageService } from 'ngx-webstorage';

@Component({
  selector: 'app-dashboard-home',
  templateUrl: './dashboard-home.component.html',
  styleUrls: ['./dashboard-home.component.scss']
})
export class DashboardHomeComponent implements OnInit {


  account: any = {};
  points: any = {};
  is_loaded: boolean;

  constructor(
    private sessionStorageService: SessionStorageService,
    private customerService: CustomerService,
    private authService: AuthService,
    private superPointsService: SuperpointsService) { }

  ngOnInit() {
    this.getAccount();
    this.getPoints();
  }

  public getAccount() {
    var account = this.authService.getAccount();

    this.customerService.get(account.token.access_token).subscribe((response) => {
      this.account = response;
    });
  }

  public getPoints() {
    this.superPointsService.getBalance().subscribe((response) => {
      this.points = response;
      this.is_loaded = true;
    });
  }
}
