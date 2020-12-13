
module.exports = {
    main: {
        options: {
            match: [
                {
                    "plugins.min.js": "content/js/plugins.min.js",
                    "accounts.min.js": "content/js/accounts.min.js",

                    "favicon.ico": "content/images/favicon.ico",

                    "plugins.min.css": "content/css/plugins.min.css",
                    "accounts.min.css": "content/css/accounts.min.css"
                }
            ],
            replacement: "md5"
        },
        files: {
            src: [
                "../../publish/ui/hagrid.ui.login/**/*.html",
                "../../publish/ui/hagrid.ui.consumer/**/*.html"
            ]
        }
    }
}