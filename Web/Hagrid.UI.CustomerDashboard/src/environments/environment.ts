// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `.angular-cli.json`.

export const environment = {
    production: false,
    jsClient: '9C7BCF16-C61C-4E13-8F13-9F57C40085D2',
    urls: {
        accountCustomerDashboardURL: 'http://localhost:666',
        accountEndpoint: 'http://localhost:55888/v2',
        logisticsEndpoint: 'http://integration-staging.genstore.com.br/logistics',
        accountSiteURL: 'http://localhost:55777',
        accountAdminURL: 'http://accounts-staging.genstore.com.br/admin',
        rpayJS: 'https://static-sandbox.genpay.com.br/rpayjs/rpay-latest.dev.min.js'
    }
};
