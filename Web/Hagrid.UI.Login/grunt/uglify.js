
module.exports = {
    options: {
        mangle: true
    },
    app: {
        files: {
            "content/js/<%= pkg.fileName %>.min.js": [
                "app/app.js",
                "app/app.*.js",
                "app/json/*.js",
                "app/services/*.js",
                "app/directives/*.js",
                "app/controllers/*.js"
            ]
        }
    }
}