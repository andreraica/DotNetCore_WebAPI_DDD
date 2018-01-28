# dotnet-core

Projeto DotNetCore com API e Token 

<h1>Compilação</h1> 

<p>
cd ...\dotnet-core\back-end\src\Middleware\Api <br />
dotnet restore <br />
dotnet build <br />
code .
</p>
<p>
Visual Studio Code <br />
--Tab Debug (.NET Core Launch (web)) <br />
--Play
</p>

<p>

Só executa um migration com start project sendo um app executavel

Migrations
Infrastructure/Data - change the app to netstandart16
dotnetrestore
dotnet ef migrations add initial -s ..\..\Middleware\Api\Api.csproj
dotnet ef database update -s ..\..\Middleware\Api\Api.csproj
</p>


<p>
    Mais detalhes pasta Docs...
</p>