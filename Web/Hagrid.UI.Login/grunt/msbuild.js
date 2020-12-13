

function getConfigs(config) {
    return {
        src: ['../../API/Hagrid.API/Hagrid.API.csproj'],
        options: {
            projectConfiguration: config,
            targets: ['Clean', 'Rebuild'],
            version: 15.0,
            maxCpuCount: 4,
            buildParameters: {
                WarningLevel: 2
            },
            nodeReuse: true,
            customArgs: [],
            verbosity: 'minimal'
        }
    }
}

module.exports = {

    'accounts-development': getConfigs('Dev'),
    'manager-development': getConfigs('Dev.Manager'),

    'accounts-homologation': getConfigs('Hom'),
    'manager-homologation': getConfigs('Hom.Manager'),

    'accounts-staging': getConfigs('Staging'),
    'manager-staging': getConfigs('Staging.Manager'),

    'accounts-sandbox': getConfigs('Sandbox'),
    'manager-sandbox': getConfigs('Sandbox.Manager'),

    'accounts-production': getConfigs('Release'),
    'manager-production': getConfigs('Release.Manager')
}
