
Inicjalizacja Projektu

Krok 1 – Inicjalizacja projektu: Utworzyliśmy szkielet ASP .NET Core Web API przy pomocy dotnet new webapi i skonfigurowaliśmy kontrolę wersji (Git + .gitignore), co jest standardowym punktem startowym każdego projektu .NET 
WireFuture
.

Krok 2 – Struktura repozytorium: Wydzieliliśmy katalogi /src, /tests i /docker, wprowadziliśmy GitHub Flow dla rozgałęzień funkcjonalnych i pull requestów, zapewniając przejrzystość i powtarzalność procesu developmentu 
Docker Documentation
.

Krok 3 – Docker Compose: Stworzyliśmy docker-compose.yml z usługami api (ASP .NET Core) oraz db (MySQL), definiując sieć i wolumen danych, co umożliwia łatwe uruchamianie całości w jednym poleceniu 
Docker Documentation
.

Krok 4 – ORM – EF Core + MySQL: Zainstalowaliśmy Pomelo.EntityFrameworkCore.MySql, skonfigurowaliśmy EnergyDbContext w Program.cs z UseMySql(...) i wykonaliśmy pierwszą migrację, co pozwala na mapowanie encji na tabele MySQL 
WireFuture
.

Krok 5 – Modele domenowe: Zdefiniowaliśmy klasy Country, EnergyBalanceRecord i SourceMetadata wraz z relacjami one‑to‑many, używając Fluent API i/lub Data Annotations, co stanowi odzwierciedlenie logicznej warstwy danych 
Code Maze
.

Krok 6 – JWT Authentication: Dodaliśmy pakiet Microsoft.AspNetCore.Authentication.JwtBearer (wersja 8.x), skonfigurowaliśmy middleware JWT w Program.cs, wdrożyliśmy serwis AuthService oraz kontroler AuthController dla /register i /login, co zabezpiecza aplikację tokenami JWT 
InfoWorld
.

Swagger / OpenAPI: Uzupełniliśmy Program.cs o AddSwaggerGen() i middleware UseSwagger()/UseSwaggerUI(), dzięki czemu mamy automatyczną dokumentację API i wygodny interfejs testowy 
Code Maze
.