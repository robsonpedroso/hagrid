Introduction
============

This is the **Angular** version of **Hagrid UI Admin** -- what is a fully responsive admin template. Based on **[Bootstrap 3](https://github.com/twbs/bootstrap)** framework. Highly customizable and easy to use. Fits many screen resolutions from small mobile devices to large desktops. Check out the live preview now and see for yourself.

Installation
------------

- Clone to your machine
- Install Angular 2 Client.
```bash
npm install -g @angular/cli
```
- Clone the repository
```bash
git clone https://github.com/robsonpedroso/hagrid.git Hagrid
```

- Install the packages
```bash
cd Hagrid
npm install
```

Running the application
------------
- On the folder project
```
ng serve
```
- For admin page Navigate to [http://localhost:4201/admin](http://localhost:4201/admin)

Opening the Angular 2 project in Visual Studio
------------
[Angular 2 and TypeScript in Visual Studio](https://blogs.msdn.microsoft.com/visualstudio/2015/03/12/a-preview-of-angular-2-and-typescript-in-visual-studio/)

TypeScript for Visual Studio 2013: https://www.microsoft.com/en-us/download/details.aspx?id=48739


How to deploy
------------

To publish Hagrid you need use npm run.

####Using The Command Line:

**npm**

To publish in dev, staging, sandbox or production:

```
npm run pub:<environment>

npm run pub:[dev,staging,sandbox,prod,production]

npm run pub:prod
```


The output files are `./dist` folder.

Browser Support
---------------
- IE 9+
- Firefox (latest)
- Chrome (latest)
- Safari (latest)
- Opera (latest)

 Credits
-------------
[Angular2-AdminLTE](https://github.com/csotomon/Angular2-AdminLTE/)
