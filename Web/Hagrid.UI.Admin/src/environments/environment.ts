// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
	production: false,
	is_manager: false,
	theme: 'skin-blue',
	client_id: 'CCE5A0A6-969D-4BA0-98D5-E65F117C704E',
	create_user_rmail_template: 101,
	urls: {
		accountEndpoint: 'http://localhost:55888/v2',
		accountSiteURL: 'http://localhost:55777',
        accountAdminURL: 'http://localhost:4201',
        logisticsEndpoint: 'http://localhost:59824/'
	}
};
