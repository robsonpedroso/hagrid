
module.exports = {
    options: {
        shorthandCompacting: false,
        roundingPrecision: -1,
        noAdvanced: true
    },
    app: {
        files: {
            "content/css/<%= pkg.fileName %>.min.css": [ "content/css/accounts/*.css" ]
        }
    },

    dep: {
        files: {
            "content/css/plugins.min.css": [ "content/css/plugins/*.css" ]
        }
    }
}