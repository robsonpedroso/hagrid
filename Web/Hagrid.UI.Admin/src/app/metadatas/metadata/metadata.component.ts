import { Component, OnInit, Input } from '@angular/core';
import { NotificationsService } from 'angular2-notifications';
import { ActivatedRoute } from '@angular/router';
import { MetadatasService } from '../metadatas.service';

@Component({
	selector: 'metadata',
	templateUrl: './metadata.component.html'
})
export class MetadataComponent implements OnInit {

	public metadatas: any = {};

	@Input() set value(metadatas: any) {
		if (metadatas) {
			this.metadatasService.buildAttributesMetadata(metadatas);
		}
		this.metadatas = metadatas;
	}

	constructor(
		private route: ActivatedRoute,
		private notify: NotificationsService,
		private metadatasService: MetadatasService) { }

	ngOnInit() { }

	public saveMetadata(hideNotify?: boolean): void {
		let metadata_field = this.metadatas[0];
		this.metadatasService.saveValue(metadata_field.type, this.route.snapshot.params.id, this.metadatas).subscribe(() => {
			if (!hideNotify) {
				this.notify.success('Sucesso!', 'Metadados salvo com sucesso =)');
			}
		});
	}
}
