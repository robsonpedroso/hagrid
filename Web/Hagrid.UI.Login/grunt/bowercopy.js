
module.exports = {
    options: {
        clean: true
    },
    css: {
        options: {
            destPrefix: "content/css/plugins"
        },
        files: {
            "01.bootstrap.min.css": "bootstrap/dist/css/bootstrap.min.css",
            "02.font-awesome.min.css": "font-awesome/css/font-awesome.min.css",
            "03.loading-bar.min.css": "angular-loading-bar/build/loading-bar.min.css"
        }
    },
    js: {
        options: {
            destPrefix: "content/js/plugins"
        },
        files: {
            "01.jquery.min.js": "jquery/dist/jquery.min.js",
            "02.angular.min.js": "angular/angular.min.js",
            "03.angular-messages.min.js": "angular-messages/angular-messages.min.js",
            "04.angular-route.min.js": "angular-route/angular-route.min.js",
            "05.ngStorage.min.js": "ngstorage/ngStorage.min.js",
            "06.ngMask.min.js": "ngMask/dist/ngMask.min.js"
        }
    },
    fonts: {
        options: {
            destPrefix: "content/fonts/"
        },
        files: {
            "FontAwesome.otf": "font-awesome/fonts/FontAwesome.otf",
            "fontawesome-webfont.eot": "font-awesome/fonts/fontawesome-webfont.eot",
            "fontawesome-webfont.svg": "font-awesome/fonts/fontawesome-webfont.svg",
            "fontawesome-webfont.ttf": "font-awesome/fonts/fontawesome-webfont.ttf",
            "fontawesome-webfont.woff": "font-awesome/fonts/fontawesome-webfont.woff",
            "fontawesome-webfont.woff2": "font-awesome/fonts/fontawesome-webfont.woff2"
        }
    }
}
