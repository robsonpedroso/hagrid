import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
    selector: 'app-master-footer',
    templateUrl: './master-footer.component.html'
})
export class MasterFooterComponent implements OnInit {

    public year: number;
    
    constructor(private route: ActivatedRoute) { }

    ngOnInit() {
        this.year = new Date().getFullYear();
    }
}
