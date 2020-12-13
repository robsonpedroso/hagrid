
function getConfigsWeb(root_dest) {
    return [
        {
            expand: true,
            src: [
                "*.html",
                "app/**/*.html",
                "content/css/*.min.css",
                "content/js/*.min.js",
                "content/fonts/**",
                "content/images/**"
            ],
            dest: root_dest,
            filter: "isFile"
        }
    ]
}

function getConfigs(root_dest) {
    return [
        {
            expand: true,
            src: [
                "../../API/Hagrid.API/bin/*.*",
                "!../../API/Hagrid.API/bin/*.config",
                "!../../API/Hagrid.API/bin/*.xml",
                "../../Migrate/migrate.exe",
                "../../Migrate/Migrate.bat",
                "../../Migrate/Migrate_rollback.bat"
            ],
            dest: root_dest + "/bin/",
            filter: "isFile",
            flatten: true
        },
        {
            expand: true,
            src: [
                "../../Core/Hagrid.Core.Repositories/Repositories/EF/Migrations/Scripts/*.sql"
            ],
            dest: root_dest + "/bin/Repositories/EF/Migrations/Scripts/",
            filter: "isFile",
            flatten: true
        },
        {
            expand: true,
            src: ["../../API/Hagrid.API/Web.config"],
            dest: root_dest,
            filter: "isFile",
            flatten: true
        }
    ]
}

module.exports = {
    accounts: {
        files: [
            getConfigs("../../publish/Hagrid.API"),
            getConfigsWeb("../../publish/ui/hagrid.ui.login")
        ]
    }
 };