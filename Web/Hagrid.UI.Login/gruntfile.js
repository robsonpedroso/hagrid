module.exports = function (grunt) {

    grunt.initConfig({
        pkg: grunt.file.readJSON('package.json'),
        concat: require('./grunt/concat'),
        uglify: require('./grunt/uglify'),
        cssmin: require('./grunt/cssmin'),
        watch: require('./grunt/watch'),
        copy: require('./grunt/copy'),
        ngconstant: require('./grunt/ngconstant'),
        bowercopy: require('./grunt/bowercopy'),
        cachebreaker: require('./grunt/cachebreaker'),
        msbuild: require('./grunt/msbuild')
    });

    grunt.loadNpmTasks('grunt-contrib-concat');
    grunt.loadNpmTasks('grunt-contrib-uglify');
    grunt.loadNpmTasks('grunt-contrib-cssmin');
    grunt.loadNpmTasks('grunt-contrib-watch');
    grunt.loadNpmTasks('grunt-contrib-copy');
    grunt.loadNpmTasks('grunt-ng-constant');
    grunt.loadNpmTasks('grunt-bowercopy');
    grunt.loadNpmTasks('grunt-cache-breaker');
    grunt.loadNpmTasks('grunt-msbuild');

    grunt.registerTask('default', [
        "ngconstant:accounts-localhost",
        "concat:app",
        "cssmin:app"
    ]);

    grunt.registerTask('pub-prod', [
        'concat:dep',
        'cssmin',

        'msbuild:accounts-production',
        'ngconstant:accounts-production',
        'uglify',
        'copy:accounts',

        'cachebreaker'
    ]);

    grunt.registerTask('pub', 'Generate files for publish', function (env, build) {

        if (arguments.length === 0) {
            grunt.log.writeln("Nenhum ambiente informado! Passe o ambiente por parametro.\n\nEx.: grunt pub:staging");
            return;
        }

        if (env !== "staging" && env !== "sandbox" && env !== "production" && env !== "prod") {
            grunt.log.writeln("Ambiente Informado não existe.\n\nEx.: grunt pub:staging, grunt pub:sandbox ou grunt pub:production");
            return;
        }

        if (env == "prod")
            env = "production";

        if (typeof (build) == 'undefined')
            build = false;

        grunt.log.writeln("\n");
        grunt.log.writeln("Ambiente Informado: " + env);
        grunt.log.writeln("Compilar Projeto API: " + build);

        var task = [];

        if (build)
            task.push('msbuild:accounts-' + env);

        task = task.concat([
            'concat:dep',
            'cssmin',
            'ngconstant:accounts-' + env
        ]);

        if (env == 'production')
            task.push('uglify');
        else
            task.push('concat:app');

        task = task.concat([
            'copy:accounts',
            'cachebreaker'
        ]);

        grunt.task.run(task);
    });
};