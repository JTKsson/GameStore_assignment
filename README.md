<h2>För godkänt</h2> 

<h3>Skapa en webapp:</h3>

1. Skapa en ny Web App genom Azure-portalen. 
2. välj subscription och resource Group (jag gjorde en ny för uppgiften).
3. Ge projectet ett logiskt namn (t.ex gamestore-asignment), välj runtime stack (.Net 8 för uppgiften), OS (linux) och region (sweden central, North Europe eller West Europe).
4. Välj plan flr operativsystemet och sedan Pricing Plan/SKU (F1 för gratis användning). 
5. Vi kan för detta projekt ignorera flikarna Database, Deployment och Networking. Deployment måste vi sätta upp efter att vi skapat Webapplikationen (om man väljer windows som OS eller en dyrare plan så kan man göra det steget i samband med Web App).
6. Under fliken Monitor + Secure behöver vi bara se till att Enable Application Insight är "Yes". Ignorera fliken Tags
7. Review + create: kolla så att allt stämmer (framför allt att du har Free SKU). Sedan tryck Create.

<h3>Deploya kod till webappen:</h3>

1. När webappen har skapats, tryck på Go to resource eller sök på Web Services i sökrutan så kommer du få fram en lista på alla webbapplikationer. Klicka på den du precis skapat
2. Väl inne i din webbapp så har du en lista med inställningar till vänster. Vi vill gå in på Deployment > Deployment center. Väl där inne kan vi bestömma en kodkälla (code source) för att sätta upp Continuous Deployment (jag väljer Github och Github actions). 
3. Välj den plattform du har valt att hantera ditt kodrepo (du kan behöva logga in). 
4. Välj organisation, repository och branch (i detta fall väljer du main/master)
5. Authentication settings can du låta vara på default. 
6. Sedan trycker du Save. Den knappen hittar du på raden över rutan med deployment infon.
7. Deployment kan ta en stund. När den väl är klar, gå till Overview och öppna din Default domain. Du bör se din webbapplikation i full rulle.

Länk till webappen: https://gamestore-assignment.azurewebsites.net/

Om du stöter på problem i din deployment och har valt Github, gå till din Github profil, klicka in på repot du föröker deploya och tryck på fliken Actions. Det är Github actions som bygger applicationen till körbar kod, så om det blir något fel så hittar du loggen här och inte i Azure. 
Det går också att komma till relevant Action genom Deployment Center under Webapplikationen i Azure och tryck på logs-länken, så skickas du till Github Actions.

<h3>Loggning och övervakning:</h3>

Med hjälp av Application Insights, som vi valde att använda när vi skapade vår Web App, kan vi övervaka trafiken i appen och se över diverse logfiler som kartlägger viss aktivitet.
1. Åter igen när du har översikten över din webapp, finns en menyrad där du hittar Monitoring. Där har du en överblick av applikationens drift
2. Till vänster i menylistan, där vi har en länk till Activity log. Här kan du se en lista över Operations som visar ändringar som händer i webapp-resursen, t.ex om prisplanen ändras, deploy, omstart osv.

<img src="https://gamestorestorage1337.blob.core.windows.net/blobs-anon/insight-data.png"/>

<h3>Implementera IAM (Identity and Access Management):</h3>

IAM tillåter dig att ge särskilld behörighet till personer inom ditt "team". Du kan sätta upp IAMs för varje resurs, eller implementera det på din resource group så kommer behörigheten innefatta alla resurser i gruppen.
Vi kommer skapa en roll, Reader, som ger användare behörighet att läsa resurserna, men inte gära några ändringar.
1. Sök på Resourse groups i sökfältet och klicka in på den resursgrupp du använder till din webapplikation.
2. Väl inne i resursgruppens översikt, i menyn till vänster hittar vi Access controll (IAM). Klicka där.
3. Här kan vi lägga till personer i roller, se vilka roller vi har applicerat och användare som har rollerna, samt lite annat mums. 
4. För att lägga till, tryck på Add i menyraden längst upp och sedan Add role assignment. 
5. Välj rollen Reader. Du bör hitta den längst upp i listan. Du kan annars söka på rollen.
6. Efter du tryckt på Reader, tryck på next. Här lägger du till vilka användare som ska få rollen. 
7. Sedan går du till Review + assign och färdigställer ändringarna.

<h3>Implementera SSL (Secure Socket Layer):</h3>
När man skapar en Web app på Azure så ingår ett grundläggande SSL-certifikat per default, även om vi använder en gratis plan, vi behöver inte göra något. Det kan vi se om vi navigerar oss till webappens default domain. I URL:ens sökfält ser vi att förfrågningar görs via https och inte http. Detta betyder att datan som skickas mellan server och klient är säkrad. 

Det vi kan göra, om vi har en betalplan, är att vi kan lägga till fler certifikat och ett eget domännamn.
1. I din översyn i Webappsresursen, i menyn till vänster, sök på SSL.
2. Då får du upp två länkar, Custom domains och Certificates.
3. Inne i Custom domains kan du lägga till egna domäner, t.ex timosgameshop.io istället för Azure förbestämda <>.azurewebsites.net
4. Inne i Certificates kan du hantera certifikat som Azure tillhandahåller eller lägga till tredje-parts certifikat.

<h2>--För väl godkänt--</h2>

Automatisk skalning:
För att hantera skalning av en resurs så kan man antingen söka på Autoscale och få en lista på tillgängliga resurser, eller navigera till web app > settings > scale out. 

I och med att vi använder Free F1 som SKU så har vi inte tillgång till automatisk skalning, vilket är rimligt eftersom vi inte betalar något för att använda tjänsten. 
För att få tillgång till automatisk skalning behöver vi använda oss av en högre SKU (Premium V2 eller Premium V3). 

Storage account och uppladdning av filer:
1. Sök på storage accounts i sökfältet i Azure-portalen.
2. Skapa en ny Storage Account
3. Välj Resource group (fördelaktigen den som du har till din webapp)
4. Välj storage account name. Detta måste vara helt unikt då den kommer bli en del av URL-länken som låter dig interagera med ditt storage.
5. Välj region (Sweden Central, North Europe eller West Europe), fördelaktigen den region som din webapp är i.
6. Vi kommer välja Azure Blob Storage som Primary Service, då vi ska lagra ostrukturerad data som bilder.
7. Performance standard, Redundancy Locally-redundant storage för billigast möjliga drift. 
8. I advanced behöver vi inte göra mycket. Per default så får vi de säkerhetsföreskrifter som behövs för stunden, t.ex säkrad trafik via https. I access tier väljer vi Cool, som ger oss billigare lagringskostnad men risk för lite högre åtkomstkostnad. 
9. I Data protection vill vi använda alla soft delete inställningar, detta låter oss ångra borttagningar av filer. Istället för att de försvinner direkt så läggs de i en papperskorg under en vald tid innan de försvinner. Inget annat behövs för stunden.
10. Encryptions default är bra, ignorera Tags. Gå till Review + create och slutför.
11. För att kunna ladda upp bilder i vårt storage account så måste vi först skapa en container. I överblicken av ditt storage, i menyn till vänster har du en flik som heter Data storage och under den Containers. Klicka på den
12. I menyraden längst upp har du +Containers. Tryck på den så får du en ruta till höger där du ger din container ett namn och tryck sedan på create. Du kommer se din container i listan av containers.
13. När du trycker på din container så får du en överblick om den. I menyraden längst upp kan du nu ladda upp filer med Upload. Åter igen får du upp en ruta till höger där du laddar upp en fil och konfigurera vissa inställningar.
14. När du har laddat upp en fil (bild i det här fallet) kan du få yttligare en överblick kring filen. Det mest intressanta just nu är URL:en för filen. Men för att kunna se filen, måste vi skapa en Shared Access Signature (SAS) token som visar att vi har behörighet att läsa filen. 
15. Under menyraden längt upp har vi några flikar, bland annat Generate SAS. Där inne kan du generera en token som tillåter dig att använda filen. Här kan du ge behörighet för nyckeln och bestämma vilket tidsspann det ska gälla. 16. När du väl har genererat token och URL får du tillgång till en URL med tillhörande token. Kopiera den och klistra in den i din webbläsare. Du bör få upp en visning av filen.

Länk utan token: https://gamestorestorage1337.blob.core.windows.net/blobs/test.png

Länk med token: https://gamestorestorage1337.blob.core.windows.net/blobs/test.png?sp=r&st=2025-04-09T18:50:03Z&se=2025-07-12T02:50:03Z&spr=https&sv=2024-11-04&sr=b&sig=uqrrSlQq3UwtK0O0UZGcXzr2r%2FMUVsMAgzqIFLyGt7A%3D

För att inte behöva skapa en SAS-token (och därmed ta bort en bit av säkerheten) kan du välja att tillåta Anonymous access till containers när du skapar en storage account och sedan tillåta anonym access när du skapar en container. På så vis kan "vem som helst" läsa filer i den containern utan en SAS-token.


Azure Key Vault:
1. Jag skapade ett nytt key vault och höll konfigurationerna enkla. Eftersom jag inte planerar att förvara något superhemligt så väljer jag standard pricing och låter bli HSM-protection. 
2. För att kunna skapa objekt i mitt key vault så behövde jag konfigurera IAM och ge mig själv rättigheterna av en Key Vault Administrator. Det räckte inte med att vara owner.
3. För att integrera Azure Key Vault i projektet använde jag mig av kodförslaget som Azure tillhandahåller när det gäller secret-objekt.
4. För uppgiftens syfte satte jag upp koden så man kan se secret genom en endpoint: https://gamestore-assignment.azurewebsites.net/api/secret

CI/CD-pipeline i Azure DevOps:
1. i och med att jag har mitt Project deployat via github så skapade jag en lokal kopia av projektet och ändrade remote origin från github till repot i Azure DevOps. (git remote set-url origin <repo länk>)
2. sedan använde jag origin-länken från Azure och pushade upp all kod till devops.
3. Jag provade CI genom att lägga till en fil lokalt och pusha till remote. funkade utmärkt.
4. När jag skulle skapa Continuous Deployment så stötte jag på problem. Det visar sig att i vårt student-subscription så ingår inte funktionaliteten att sätta upp ett CD i Azure DevOps. Vi har inte tillgång till ett så kallat Microsoft-hosted build agents (även kallat parallell jobs).
Som jag uppfattar situationen, är att Microsoft tillhandahåller ett VM som hostar pipelinen, något som vi inte får tillgång till per automatik. Jag har skickat en förfrågan om att få tillgång till ett VM för pipeline.


<img src="https://gamestorestorage1337.blob.core.windows.net/blobs-anon/error-parallellism.png"/>


EDIT:
Jag har fått en VM för Pipeline tilldelat mig, så jag har nu en lyckad pipeline. 

<img src="https://gamestorestorage1337.blob.core.windows.net/blobs-anon/Skärmbild 2025-04-15 102254.png" />












