Prompt 1


Witaj Jarvis. Mam dla ciebie dzisiaj bojowe zadanie. Wiem, że jesteś ekspertem w dziedzinie integracji systemów, dlatego cenię sobie twoją pomoc. Potrafisz integrować różne środowiska i używać różnych języków programowania. Mam bardzo ważne zadanie do wykonania.

Temat brzmi: 
Bilanse energii pozyskiwanej z paliw kopalnych i energii odnawialnej dla Polsk,i w zdanych okresach czasu, w porównaniu z innymi krajami świata.

Projekt integracji:

W ujęciu ogólnym, projekt integracji wykorzystuje różne źródła danych, w tym takie które są przestarzałe i na ich podstawie tworzy nowe, aktualne zestawy, służące do spełniania 
bieżących potrzeb. Opiera się o konsolidację odmiennych źródeł danych, wdrażanie nowych  systemów, hurtowni danych itd. .  
Projekt roszerza doświadczenie w dziedzinie implementacji rozwiązań integracyjnych. Do przygotowania projektu można wykorzystać dodatkowe, nieomawiane w toku laboratoriów technologie i narzędzia, jednak wszelkie użyte dodatki powinny jedynie dawać  nowe możliwości autorowi projektu a nie wyręczać go we wszystkim.  Projekt Integracji zawiera w sobie tworzenie aplikacji klienckich do nieznanych systemów,  jak również tworzenie własnych aplikacji serwerowych, które mogą stanowić formę agregacji i głębszej analizy danych. 

Cel projektu integracji:

Projekt powinien być odpowiedzią na problem wynikający z niejednoznaczności formatów
danych i ich opisów, stanowiący przeszkodę do tworzenia analiz, raportów, korelacji itp.
Analizy, raporty lub korelacje będące celem integracji muszą być faktycznie istotne a ich  osiągnięcie faktycznie musi być realizowane poprzez integrację heterogenicznych zasobów danych.

Wymagania funkcjonalne:

-Obsługa usługi SOAP lub REST dla pobierania danych. 
• Eksport danych w formacie XML lub JSON.
• Zapis pozyskanych danych do bazy danych aplikacji oraz odczyt danych z bazy
do aplikacji z wykorzystaniem ORM.
• Implementacja mechanizmu uwierzytelnienia i autoryzacji
• Implementacja mechanizmu transakcji w dostępie (odczyt zapis) do bazy danych.

Wymagania poza funkcjonalne do projektów integracyjnych:

- Wykorzystanie warstwy graficznej: interface użytkownika, pobieranie danych, 
wyświetlanie wyników itd.
• Funkcje usprawniające dostęp do współdzielonych zasobów (poziomy izolacji
w bazach danych).
-Stworzenie kontenera Docker obejmującego stworzoną aplikację lub zespół aplikacji 
wraz ze wszystkimi koniecznymi do działania składnikami

Przeczytaj dokładnie wszystko i dokonaj wstępnej szczegółowej analizy. Następnie będziemy mogli sobie porozmawiać jak podejść do tego tematu, jakich technologi użyć, itp. Do dzieła


Prompt 2

Krok 1: Inicjalizacja projektu
dotnet new webapi -n EnergyBalancesApi – generuje szkielet projektu Web API w .NET 8.0 
Medium
.

Dodaj plik .gitignore dla .NET i utwórz repozytorium Git.

Krok 2: Struktura repozytorium
Wydziel katalogi: /src (kod), /tests, /docker.

Użyj GitHub Flow (branch main, feature branches, pull request).

Krok 3: Docker Compose
Stwórz docker-compose.yml z dwoma usługami: api i db (MySQL) 
DEV Community
.

Zdefiniuj wolumen dla danych MySQL i sieć.

Krok 4: ORM – EF Core + MySQL
Zainstaluj NuGet: Pomelo.EntityFrameworkCore.MySql lub MySql.EntityFrameworkCore 
mysqlconnector.net
.

Skonfiguruj DbContext w Program.cs: options.UseMySql(connectionString, ServerVersion.AutoDetect(...)) 
MySQL
.

Krok 5: Modele domenowe
Zdefiniuj klasy: Country, EnergyBalanceRecord, SourceMetadata itp.

Użyj atrybutów Data Annotations lub fluent API, aby określić klucze i relacje.

Krok 6: JWT Authentication
Dodaj pakiet Microsoft.AspNetCore.Authentication.JwtBearer 
Microsoft Learn
.

W Program.cs skonfiguruj middleware: AddAuthentication().AddJwtBearer(...).

Stwórz endpointy /auth/login, /auth/register generujące tokeny.

Krok 7: Konektory REST do źródeł
Wykorzystaj HttpClientFactory lub bibliotekę Refit do wywołań API Eurostat, OWID, IEA 
Medium
.

Zaimplementuj retry policy (Polly).

Krok 8: ETL Pipeline
Zastosuj Pipeline Pattern lub ETLBox do etapów Extract→Transform→Load 
Medium
Panoply
.

Zdefiniuj kroki jako oddzielne klasy implementujące interfejsy (IExtract, ITransform, ILoad).

Krok 9: Transformacje danych
Mapuj surowe JSON/XML na obiekty domenowe przy użyciu LINQ i AutoMapper (opcjonalnie).

Konwertuj jednostki energii (ktoe, TJ) do wspólnego formatu.

Krok 10: Ładowanie do bazy
Użyj EF Core BulkExtensions lub SaveChanges() w transakcji dla batch‑insertów 
Microsoft Learn
.

Zapewnij odpowiedni poziom izolacji transakcji.

Krok 11: REST API dla wyników
Stwórz kontrolery: EnergyController, CountriesController zwracające JSON/XML.

Dodaj filtrowanie (rok, typ energii), paginację i sortowanie.

Krok 12: Logowanie i monitoring
Skonfiguruj Serilog z sinkiem do pliku i ElasticSearch/Kibana 
YouTube
.

Loguj błędy ETL i zapytania API.

Krok 13: Testy
Jednostkowe: testuj usługi ETL, transformacje, generowanie tokenów.

Integracyjne: użyj WebApplicationFactory<T> i in‑memory MySQL lub TestContainer 
Microsoft Learn
.

Krok 14: CI/CD
GitHub Actions: buduj obraz Docker, uruchamiaj testy, publikuj do Docker Hub lub Azure Container Registry 
Microsoft Learn
.

Krok 15: Wdrożenie
Uruchom docker-compose up -d na serwerze.

(Opcjonalnie) Kubernetes manifests / Helm chart.


https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/NRG_BAL_C?unit=KTOE&geo=PL,DE,FR&time=2010&lang=EN


https://ec.europa.eu/eurostat/api/dissemination/statistics/1.0/data/nrg_bal_c?unit=KTOE&geo=PL&time=2010&lang=EN


Witaj Jarvis. Jesteś ekspertem i prawdziwym mistrzem w dziedzinie integracji systemów. Wiem, że potrafisz bez żadnych kłopotów integrować różne środowiska i wykorzystywać różne technologie programistyczne. Mam przygotowany projekt razem z gotowym backend. Chciałbym jednak rozpocząć od podania ci wszystkich wymagań projektowych, abyś mógł zobaczyć i dokładnie przeanalizować cały projekt. 

Temat projektu brzmi: 

Bilanse energii pozyskiwanej z paliw kopalnych i energii odnawialnej dla Polsk,i w zdanych okresach czasu, w porównaniu z innymi krajami świata.

Teraz przedstawiam ci wszystkie wymagania funkcjonalne oraz niefunkcjonalne.

Wymagania funkcjonalne do projektów integracyjnych:
• Obsługa usługi SOAP lub REST dla pobierania danych.
• Eksport danych w formacie XML lub JSON.
• Zapis pozyskanych danych do bazy danych aplikacji oraz odczyt danych z bazy
do aplikacji z wykorzystaniem ORM.
• Implementacja mechanizmu uwierzytelnienia i autoryzacji
• Implementacja mechanizmu transakcji w dostępie (odczyt zapis) do bazy danych.

Wymagania poza funkcjonalne do projektów integracyjnych:
• Wykorzystanie warstwy graficznej: interface użytkownika, pobieranie danych,
wyświetlanie wyników itd.
• Funkcje usprawniające dostęp do współdzielonych zasobów (poziomy izolacji
w bazach danych).
• Stworzenie kontenera Docker obejmującego stworzoną aplikację lub zespół aplikacji
wraz ze wszystkimi koniecznymi do działania składnikami. 

Teraz chcę, abyś dokładnie z pełną precyzją i przy wykorzystaniu twojego doświadczenia oraz ogromnej ekspertyzy dokonał szczegółowej anaizy tego tematu oraz wymagań. Będe chciał rozegrać to w następujący sposób:

1. Przeprowadzisz pełną analizę tematu oraz wymagań funkcjonalnych oraz niefunkcjonalnych. Dzięki temu będziesz miał pełne pojęcie jak projektować aplikacje.
2. Następnie przejdę do pokazania ci mojej struktury plików w projekcie backend. Pokaże ci gdzie znajduje się dany plik. Backend zrobiłem w technologi .NET 8.0 przy użyciu C#. Zintegrowałem się z mySQL. Reszte rzeczy dowiesz się już za chwile. Podsumowując punkt 2 to będzie moje przedstawienie całej struktury.
3. W punkcie trzecim przekaże ci kody ze wszystkich klas, które stworzyłem. Wszystko, czego potrzebujesz zostanie umieszczone w naszej konwersacji, abyś mógł z pełną spójnością oraz solidnością dokonać profesjonalnej szczegółowej analizy.
4. Po wysłaniu ci całej struktury plików oraz kodu, będziesz miał wiedzę na temat tematu, wymagań funkcjonalnych oraz niefunkcjonalnych, mojej struktury plików oraz wszystkich klas i funkcjonalności. Wtedy będziemy mogli porozmawiać na temat stworzenia części front-end, która będzie idealnie współgrała z resztą?

Gotowy? Ruszaj do analizy 