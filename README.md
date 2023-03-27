# Introduction 

Have ever wonder how to integrate you system with a3innuva and send the invoices to be posted.

With this hands on lab you will discover how to create a Migration Set with the a3innuva importia SDK, upload it to a3innuva manually and the send it directly from you application to a3innuva using the a3innuva | importia API.


# Getting Started

To get started with this hands on lab you will need the following requirements.

1.	<a href="https://visualstudio.microsoft.com/es/vs/community/">Microsoft Visual Studio 2022 Community Edition</a>

    * ASP.NET and Web Development toolkit installed

2.	<a href="https://a3innuva.wolterskluwer.es/">Wolters Kluwer a3innuva</a>
3.	<a href="https://a3developers.wolterskluwer.es/">Wolters Kluwer Developer account</a>
4.	<a href="https://a3developers.wolterskluwer.es/a3innuva-contabilidad-start">Wolters Kluwer Outh Client</a>

# What will be built on the hands on lab

This hands on lab start from a project already configured to perform the authentication on the Wolters Kluwer Identity Platform.

* You will start by [updating the application credentials](doc/update_application_credentials.md) to allow the application start an authentication flow in the Wolters Kluwer Identity platform

* [Generate an importia migration set](doc/generate_a3innuva_migration_set.md), download it and upload it to a3innuva manually

* [Upload the importia migration set](doc/upload_a3innuva_migration_set.md) generated to a3innuva using the a3innuva | importia API.
