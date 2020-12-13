import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs/Observable';

@Injectable()
export class MetadatasService {

	constructor(private http: HttpClient) { }

	public MetadataFormats: any[] = [
		{ id: '', text: 'Selecione...' },
		{ id: 1, text: 'Texto' },
		{ id: 2, text: 'Inteiro' },
		{ id: 3, text: 'Decimal' },
		{ id: 4, text: 'Booleano' },
		{ id: 5, text: 'JSON' }
	];

	public save(metadata_field: any): any {
		return this.http.post('/metadata', metadata_field);
	}

	public get(id: string): Observable<any> {
		return this.http.get(`/metadata/${id}/detail`);
	}

	public search(filters: any): Observable<any> {
		return this.http.post('/metadata/search', filters);
	}

	public delete(code: string, type: string): Observable<any> {
		return this.http.delete(`/metadata/${type}/${code}`);
	}

	public saveValue(type: string, referenceCode: string, metadatas: any): Observable<any> {
		return this.http.put(`/metadata/${type}/${referenceCode}/value`, metadatas);
	}

	public buildAttributesMetadata(metadatas): any {

		metadatas.forEach(meta => {

			var attributes: any = {
				name: meta.json_id,
				class: 'form-control',
				type: 'text'
			};

			switch (meta.type) {
				case 1:
					attributes.type_name = 'Loja';
					break;
				case 2:
					attributes.type_name = 'Membro';
					break;
				default:
					attributes.type_name = 'Indefinido';
					break;
			}

			switch (meta.format) {
				case 1:
					attributes.format_name = 'Texto';
					attributes.icon = 'fa-font';
					break;
				case 2:
					attributes.format_name = 'Inteiro';
					attributes.type = 'text';
					attributes.mask = '0000000000000000000000000';
					attributes.icon = 'fa-calculator';
					break;
				case 3:
					attributes.format_name = 'Decimal';
					attributes.type = 'number';
					attributes.icon = 'fa-gg';
					if (meta.value) {
						meta.value = meta.value.toString();
					}
					break;
				case 4:
					attributes.format_name = 'Booleano';
					attributes.type = 'checkbox';
					attributes.is_check = true;
					attributes.icon = 'fa-toggle-on';
					delete attributes.class;
					delete attributes.placehold;
					break;
				case 5:
					attributes.format_name = 'JSON';
					attributes.icon = 'fa-code';
					attributes.is_json = true;
					if (!meta.validator) {
						meta.validator = { schema: {} }
					}
					else if (meta.validator && !meta.validator.schema) {
						meta.validator.schema = {};
					}

					break;
				default:
					attributes.format_name = 'Indefinido';
					attributes.name = 'Indefinido';
					attributes.icon = 'fa-warning';
					break;
			}

			meta.attributes = attributes;
		});
	}
}
