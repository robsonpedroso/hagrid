import { Component, OnInit } from '@angular/core';
import { OrdersService, ToolsService } from '../../../shared/services/core';
import { FormControl } from '@angular/forms';
import {MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
@Component({
  selector: 'app-dashboard-orders',
  templateUrl: './dashboard-orders.component.html',
  styleUrls: ['./dashboard-orders.component.scss']
})
export class DashboardOrdersComponent implements OnInit {

  startDate = null;
  endDate = null;
  is_loaded: boolean = false;
  transitions: any;
  p: number = 1;
  baseStartDate= null;
  baseEndDate= null;
  search: any =
    {
      "skip": "0",
      "take": "10"
    };

  constructor(private service: OrdersService, private toolsService: ToolsService) {

  }

  ngOnInit() {
    this.baseStartDate = new Date();
    this.baseStartDate.setDate(this.baseStartDate.getDate() - 30 );
    this.startDate = new FormControl(this.baseStartDate);

    this.baseEndDate = new Date();
    this.endDate = new FormControl(this.baseEndDate);

    this.search.start_date = this.baseStartDate;
    this.search.end_date = this.baseEndDate;

    this.getOrders(this.search);
  }

  public filterOrders() {
    this.getOrders(this.search);
  }

  public getOrders(_search: any) {

    if(this.startDate == null){
      _search.start_date = this.baseStartDate;
    } else {
      _search.start_date = this.startDate.value;
    }

    if(this.endDate == null){
      _search.end_date = this.baseEndDate;
    } else {
      _search.end_date = this.endDate.value;
    }

    this.service.getOrders(_search).subscribe(response => {
      this.p = response.skip + 1;
      this.transitions = response;

      if (this.transitions.total_result > 0)
        this.is_loaded = true;
    });
  }

  public getStatusDescription(status: string): string {
    return this.toolsService.getStatusDescription(status);
  }

  public getPaymentsMethodDescription(method: string): string {
    return this.toolsService.getPaymentsMethodDescription(method);
  }

  public getPage(page: number) {
    this.search.skip = page - 1;
    this.getOrders(this.search);
  }
}
