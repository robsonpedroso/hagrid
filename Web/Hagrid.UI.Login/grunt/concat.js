
module.exports = {
    options: {
        separator: "\n",
        stripBanners: true
    },
    app: {
        options: {
            banner: "/*\n <%= pkg.fileName %>\n v<%= pkg.version %>\n <%= grunt.template.today('yyyy-mm-dd') %>\n*/\n"
        },
        src: [
            "app/app.js",
            "app/app.*.js",
            "app/json/*.js",
            "app/services/*.js",
            "app/directives/*.js",
            "app/controllers/*.js"
        ],
        dest: "content/js/<%= pkg.fileName %>.min.js"
    },
    dep: {
        src: ["content/js/plugins/*.js"],
        dest: "content/js/plugins.min.js"
    }
}