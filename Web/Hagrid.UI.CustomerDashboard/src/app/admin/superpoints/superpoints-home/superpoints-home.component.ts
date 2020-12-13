import { Component, OnInit } from '@angular/core';
import { SuperpointsService } from '../../../shared/services/superpoints.service';
import {MatTabsModule, MatDatepickerModule, MatFormFieldModule, MatInputModule} from '@angular/material';
import { MatMomentDateModule } from '@angular/material-moment-adapter';
import {FormControl, Validators} from '@angular/forms';

@Component({
  selector: 'app-superpoints-home',
  templateUrl: './superpoints-home.component.html',
  styleUrls: ['./superpoints-home.component.scss']
})
export class SuperpointsHomeComponent implements OnInit {


  filters: any = {};
  points: any = {};
  extract: any = {};
  startDate = null;
  endDate = null;
  initialStartDate = null;
  initialEndDate = null;
  is_loaded: boolean = false;
  date_now:any = Date.now();

  constructor(
    private superPointsService: SuperpointsService) {}

  ngOnInit() {

    this.initialStartDate = new Date();
    this.initialStartDate.setDate( this.initialStartDate.getDate() - 30 );
    this.startDate = new FormControl(this.initialStartDate);

    this.initialEndDate = new Date();
    this.endDate = new FormControl(this.initialEndDate);

    this.filters = {
      start_date: this.initialStartDate,
      end_date: this.initialEndDate,
      extract_type: 0,
      skip: 0,
      take: 10
    };

    this.getBalance();
  }

  public isCredit(status: any): boolean {
    let result = false;
    switch (status.toLowerCase()) {
      case "available":
      case "locked":
        result = true;
        break;
      default:
        break;
    }

    return result;
  }

  public getBalance() : void {
    this.superPointsService.getBalance().subscribe((response) => {
      this.points = response;
      this.is_loaded = true;
    });
  }

  public search(currentPage): void {

    if(this.startDate == null){
      this.filters.start_date = this.initialStartDate;
    } else {
      this.filters.start_date = this.startDate.value;
    }

    if(this.endDate == null){
      this.filters.end_date = this.initialEndDate;
    } else {
      this.filters.end_date = this.endDate.value;
    }

    this.filters.skip = currentPage -1;
    this.superPointsService.getExtract(this.filters).subscribe((response) => {
      this.extract = response;
      this.extract.current_page = this.extract.skip + 1;
    });
  }
}
