import { Component, OnInit, ViewChild } from '@angular/core';
import { StoresService } from '../stores.service';
import { NotificationsService } from 'angular2-notifications';
import { ActivatedRoute } from '@angular/router';
import { MetadataComponent } from '../../metadatas/metadata/metadata.component';
import { AuthPermissionService } from '../../shared/auth/auth-permission.service';
import { Keys } from '../../shared/models/keys';

@Component({
	selector: 'app-store-detail',
	templateUrl: './store-detail.component.html',
	styles: []
})
export class StoreDetailComponent implements OnInit {

	@ViewChild('metadata') metadataComponent: MetadataComponent;

	public store: any = {};
	public is_loaded: boolean = false;
	public storeCodeEncripted: string;
	public secretPhrase: string;
	isCopied: boolean = false;
	currentStoreDetailPermission: any = {};
	currentStoreAppPermission: any = {};

	constructor(
		private storeService: StoresService,
		private notify: NotificationsService,
		private route: ActivatedRoute,
		private authPermissionService: AuthPermissionService) { }

	public ngOnInit() {
		this.currentStoreDetailPermission = this.authPermissionService.getCurrentPermission(Keys.StoresModule.Stores);
		this.currentStoreAppPermission = this.authPermissionService.getCurrentPermission(Keys.StoresModule.Permissions);
		this.get();
	}

	public get(): void {
		this.storeService.get(this.route.snapshot.params.id).subscribe(
			response => {
				this.store = response;
				this.store.logo = this.store.logo.concat("?", new Date().getTime());
				this.is_loaded = true;

				this.showApplicationStore(this.store.code)
			}
		);
	}

	public showApplicationStore(storeCode: string)
	{
		if (this.currentStoreAppPermission.View)
		{
			this.storeService.getApplicationStore(storeCode).subscribe(
				response => {
					this.store.applications = response.applications;
				}
			);
		}
	}

	public getLinkEncripted() {
		this.storeService.getLinkEncripted(this.route.snapshot.params.id, this.secretPhrase).subscribe(
			response => {
				this.storeCodeEncripted = response;
				this.isCopied = false;
			}
		);
	}

	public update() {
		if (this.store != null) {
			this.storeService.update(this.store).subscribe(
				response => {
					this.metadataComponent.saveMetadata(true);
					this.notify.success("Sucesso!", "Loja atualizada com sucesso!");
				}
			);
		}
	}

	public fileUpload(event) {
		let fileList: FileList;

		if (event.dataTransfer != null)
			fileList = event.dataTransfer.files;
		else if (event.target != null)
			fileList = event.target.files;

		if (fileList.length > 0) {
			let file: File = fileList[0];

			this.storeService.uploadLogo(this.route.snapshot.params.id, file).subscribe(
				response => {
					this.store.logo = this.store.logo.concat("?", new Date().getTime());
					this.notify.success("Sucesso!", "Logo atualizado com sucesso!");
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
}
