import { Component, OnInit } from '@angular/core';
import { SuperpointsService } from '../../../shared/services/superpoints.service';

@Component({
  selector: 'app-superpoints-box',
  templateUrl: './superpoints-box.component.html',
  styleUrls: ['./superpoints-box.component.scss']
})
export class SuperpointsBoxComponent implements OnInit {

  points: any = {};
  is_loaded: boolean = false;

  constructor(private superPointsService: SuperpointsService) { }

  ngOnInit() {
    this.getBalance();
  }

  public getBalance() : void {
    this.superPointsService.getBalance().subscribe((response) => {
      this.points = response;
      this.is_loaded = true;
    });
  }

}
