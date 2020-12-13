import { Component, OnInit, AfterViewInit, ElementRef } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { environment } from "../../../environments/environment";


@Component({
  selector: 'app-master-footer',
  templateUrl: './master-footer.component.html',
  styleUrls: ['./master-footer.component.scss']
})
export class MasterFooterComponent implements OnInit, AfterViewInit {
  public year: number;

  constructor(
    private route: ActivatedRoute,
    private elementRef: ElementRef) { }

  ngOnInit() {
    this.year = new Date().getFullYear();
  }

  ngAfterViewInit() {
    var s=document.createElement("script");
    s.type="text/javascript";
    s.src=environment.urls.rpayJS;

    this.elementRef.nativeElement.appendChild(s);
  }
}
