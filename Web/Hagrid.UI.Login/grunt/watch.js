
module.exports = {
    scripts: {
        files: [
            "app/app.js",
            "app/json/*.js",
            "app/services/*.js",
            "app/directives/*.js",
            "app/controllers/*.js",
            "content/css/accounts/*.css",
            "grunt/ngconstant.json"
        ],
        tasks: [
            "ngconstant:manager-localhost",
            "concat:app",
            "cssmin:app"
        ]
    }
}
