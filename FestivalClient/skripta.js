var host = "https://localhost:";
var port = "44365/";
var festivaliEndpoint ="api/Festivali/";
var mestaEndpoint = "api/Mesta/";
var loginEndpoint = "api/Authentication/login";
var registerEndpoint = "api/Authentication/register";
var searchEndpoint = "api/festivali/pretraga";
var formAction = "Create";
var editingId;
var jwt_token;

// ucitavanje festivala -> loadFestivals()
function loadPage()
{
    loadFestivals();
}

// slanje HTTPGet zahteva za kolekciju Festivala
function loadFestivals()
{
    if(jwt_token)
    {
        document.getElementById("loginFormDiv").style.display = "none"
    }   
    document.getElementById("registerFormDiv").style.display = "none";

    var requestURL = host + port + festivaliEndpoint;
    console.log("Url zahteva: " + requestURL);

    var headers = {};
    if(jwt_token)
    {
        headers.Authorization = "Bearer " + jwt_token;
    }

    fetch(requestURL, {headers: headers})
    .then((response) => 
    {
        if(response.status === 200)
        {
            console.log(response);
            response.json().then(setFestivals);
        }
        else
        {
            console.log("Greska prilikom ucitavanja festivala, status: " + response.status);
            showError();
        }
    }).catch(error => console.log(error));
}

// prikaz greske => zahtev
function showError() {
	var container = document.getElementById("data");
	container.innerHTML = "";

	var div = document.createElement("div");
	var h1 = document.createElement("h1");
	var errorText = document.createTextNode("Error occured while retrieving data!");

	h1.appendChild(errorText);
	div.appendChild(h1);
	container.append(div);
}

// smestanje festivala u tabelu
function setFestivals(data)
{
    console.log(data);

    var container = document.getElementById("data");
    container.innerHTML = "";

    var div = document.createElement("div");
    div.classList = "col-10 col-md-6";
    var h1 = document.createElement("h1");
    h1.classList = "text-center";
    var naslov = document.createTextNode("Festivali");
    h1.appendChild(naslov);
    div.appendChild(h1);

    var table = document.createElement("table");
    table.classList = "table table-bordered table-stripped table-md ";
    var header = createHeader();
    table.append(header);

    var tbody = document.createElement("tbody");
    for(var i =0; i< data.length; i++)
    {
        var row = document.createElement("tr");
        row.appendChild(createTableCell(data[i].name));
        row.appendChild(createTableCell(data[i].placeName));
        row.appendChild(createTableCell(data[i].yearOfFirstEvent));
        row.appendChild(createTableCell(data[i].ticketPrice));

        if(jwt_token)
        {          
            var stringId = data[i].id.toString(); // id reda

            var btnEdit = document.createElement("button");
            btnEdit.className ="btn btn-light";
            btnEdit.name = stringId;
            btnEdit.addEventListener("click", editFestival);
            var btnEditText = document.createTextNode("[Izmena]");
            btnEdit.appendChild(btnEditText);
            var editCell = document.createElement("td");
            editCell.className = "text-center";
            editCell.appendChild(btnEdit);
            row.appendChild(editCell);
          
            var btnDelete = document.createElement("button");
            btnDelete.className = "btn btn-danger";
            btnDelete.name = stringId;
            btnDelete.addEventListener("click", deleteFestival);
            var btnDeleteText = document.createTextNode("Obrisi");
            btnDelete.appendChild(btnDeleteText);
            var deleteCell = document.createElement("td");
            deleteCell.className = "text-center";
            deleteCell.appendChild(btnDelete);
            row.appendChild(deleteCell);
        }
        tbody.appendChild(row);
    }

    table.appendChild(tbody);
    div.appendChild(table);

    container.appendChild(div);
}

// kreiranje celija th i teksta za njih sa povratnom vrednoscu:
function createTableHeaderCell(text)
{
    var cell = document.createElement("th");
    cell.classList = "text-center";
    var cellText = document.createTextNode(text);
    cell.appendChild(cellText);

    return cell;
}
// kreiranje celija td i teksta:
function createTableCell(text)
{
    var cell = document.createElement("td");
    cell.classList = "text-center";
    var cellText = document.createTextNode(text);
    cell.appendChild(cellText);

    return cell;
}
// kreiranje thead tabele: rucno pisemo id, name... u zavisnosti od tokena plus EDIT I Delete
function createHeader()
{
    var thead = document.createElement("thead");
    var row = document.createElement("tr");
    row.classList ="bg-info";
   

    row.appendChild(createTableHeaderCell("Naziv"));
    row.appendChild(createTableHeaderCell("Mesto"));
    row.appendChild(createTableHeaderCell("Godina"));
    row.appendChild(createTableHeaderCell("Cena"));

    if(jwt_token)
    {              
        row.appendChild(createTableHeaderCell("Izmena"));
        row.appendChild(createTableHeaderCell("Brisanje"));
    }
    thead.appendChild(row);
    return thead;
}

// sakriva forme i otkriva formu za registraciju
function showRegister()
{
    document.getElementById("formDiv").style.display = "none;"
    document.getElementById("registerFormDiv").style.display ="block";
    document.getElementById("loginFormDiv").style.display = "none";
    document.getElementById("logout").style.display = "none";
    document.getElementById("searchDiv").style.display = "none";
}

// slanje POST zahteva 
function registerUser()
{
    var username = document.getElementById("usernameRegister").value;
    var email = document.getElementById("emailRegister").value;
    var password =  document.getElementById("passwordRegister").value;
    var confirmPassword = document.getElementById("confirmPasswordRegister").value;

    if(validateRegisterForm(username, email, password, confirmPassword))
    {
        var requestURL = host + port + registerEndpoint;
        console.log(requestURL);
        var sendData = 
        {
            "username" :username,
            "email": email, 
            "password": password
        };

        console.log(sendData);

        fetch(requestURL, { method: "POST", headers: { 'Content-Type' : 'application/json' }, body:JSON.stringify(sendData)})    
        .then((response) =>
        {
            if(response.status === 200)
            {              
                document.getElementById("registerForm").reset(); // formu!!!
                alert("Uspesna registracija!");
                console.log("Uspesna registracija!");
                showLogin(); // prikazivanje forme za prijavu!
            }
            else
            {
                console.log("Greska prilikom registracije: " + response.status);
                console.log(response);
                alert("Greska prilikom registrovanja!");
                response.text().then(text => {console.log(text); })
                
            }
        }).catch(error => console.log(error));
    }

    return false;
}

// validacija podataka sa forme Register
function validateRegisterForm(username, email, password, confirmPassword)
{
    if(username.length === 0){
        alert("Username je obavezno polje!");
        return false;
    }
    else if(email.length === 0){
        alert("Email je obavezno polje!");
        return false;
    }
    else if(password.length === 0){
        alert("Password je obavezno polje!");
        return false;
    }
    else if(password !== confirmPassword)
    {
        alert("Lozinka i potvrda vrednosti za lozinku moraju da se poklapaju!");
        return false;
    }

    return true;
}

// prikazuje formu za login i sakriva ostale forme:
function showLogin()
{
    document.getElementById("formDiv").style.display = "none;"
    document.getElementById("loginFormDiv").style.display = "block"; 
    document.getElementById("registerFormDiv").style.display ="none";
    document.getElementById("logout").style.display ="none";
    document.getElementById("searchDiv").style.display = "none";

}

function loginUser(){

    var username = document.getElementById("usernameLogin").value;
    var password= document.getElementById("passwordLogin").value;

    if(validationLoginForm(username, password))
    {
        var url = host + port + loginEndpoint;
        var sendData = {"username": username, "password": password};
        console.log("Url zahteva: " + url);
        console.log("Objekat za slanje:");
        console.log(sendData);

        fetch(url, { method: "POST", headers: { 'Content-Type' : 'application/json' }, body:JSON.stringify(sendData)})
        .then((response) => 
        {
            if(response.status === 200)
            {
                document.getElementById("loginForm").reset();
                alert("Uspesna prijava!");
                console.log("Uspesna prijava!");
                response.json().then(function(data){
                    console.log(data);
                    document.getElementById("info").innerHTML ="Prijavljen korisnik: <i>" + data.email + "</i>.";
                    document.getElementById("logout").style.display = "block";
                    document.getElementById("searchDiv").style.display = "block";
                    document.getElementById("formDiv").style.display = "block";
      
                    jwt_token = data.token;
                    loadFestivals();
                    loadPlaces();
                });
            }else
            {
                console.log("Greska prilikom prijavljivanja na sistem: " + response.status);
                console.log(response.status);
                alert("Greska prilikom prijavljivanja na sistem!");
                response.text().then(text => { console.log(text); })
            }
        }).catch(error => console.log(error));   

    }
    return false;
}

// validacija podataka sa login forme:
function validationLoginForm(username, password)
{
    if(username.length === 0)
    {
        alert("Obavezno polje!");
        return false;
    }
    else if(password.length === 0)
    {
        alert("Obavezno polje!");
        return false;
    }

    return true;
}

// ucitavanje kolekcije mesta za dropdown => slanje GET zahteva sa tokenom
function loadPlaces()
{
    var requestURL = host + port + mestaEndpoint;
    console.log("Url zahteva: " + requestURL);

    var headers ={};
    if(jwt_token)
    {
        headers.Authorization = "Bearer " + jwt_token;
    }

    fetch(requestURL, {headers:headers})
    .then((response)=>
    {
        if(response.status === 200)
        {
            console.log("Uspesna akcija!");
            response.json().then(setPlacesInDropDown);
        }
        else
        {
            console.log("Greska prilikom ucitanja mesta: " + response.status);
            response.text().then(text => {console.log(text); })
        }
    }).catch(error => console.log(error));
}

// kolekcija Mesta i kreiranje option u dropDown:
function setPlacesInDropDown(data)
{
    console.log("Ispis podataka:");
    console.log(data);
                   
    var dropDown = document.getElementById("festivalPlace");
    dropDown.innerHTML = "";

    for(var i = 0; i< data.length; i++)
    {
        var option = document.createElement("option");
        option.value = data[i].id;
        var placeName = document.createTextNode(data[i].name);
        option.appendChild(placeName);
        dropDown.appendChild(option);
    }
}

// korisnik se odjavljuje sa sistema
function logout()
{
    jwt_token = undefined;
    document.getElementById("info").innerHTML = "";
    document.getElementById("data").innerHTML = "";
    document.getElementById("logout").style.display = "none";
    document.getElementById("searchDiv").style.display = "none";
    document.getElementById("formDiv").style.display ="none";
    document.getElementById("registerFormDiv").style.display = "none";
    document.getElementById("loginFormDiv").style.display = "block";
    loadFestivals();
}

//dodavanje festivala:
function submitFestivalForm()
{
    var naziv = document.getElementById("festivalNaziv").value;
    var cenaKarte = document.getElementById("festivalCenaKarte").value;
    var godina = document.getElementById("festivalGodina").value;
    var mesto = document.getElementById("festivalPlace").value;

    var httpAction; // post ili put
    var sendData;
    var url;

    var headers = {'Content-Type': 'application/json'};
    if(jwt_token)
    {
        headers.Authorization = "Bearer " + jwt_token;
    }

    if(formAction === "Create")
    {
        httpAction = "POST";
        url = host + port + festivaliEndpoint;
        sendData =
        {
            "Name": naziv,
            "YearOfFirstEvent" : godina,
            "PlaceId": mesto,
            "TicketPrice" : cenaKarte
        };
    }
    else
    {
          httpAction = "PUT";
          url = host + port + festivaliEndpoint + editingId.toString();
          sendData = 
          {
            "Id" : editingId,
            "Name": naziv,
            "YearOfFirstEvent" : godina,
            "PlaceId": mesto,
            "TicketPrice" : cenaKarte
          }    
    }


    console.log("Objekat za slanje: ");
    console.log(sendData);

    fetch(url, { method: httpAction, headers: headers, body: JSON.stringify(sendData) })
    .then((response) =>
    {
        if(response.status === 201 || response.status === 200 ){
            console.log("Uspesna akcija!");
            formAction = "Create";
            refreshTable();
        }
        else
        {
            console.log("Greska: " + response.status);
            alert("Greska prilikom dodavanja ili izmene festivala!");
            response.text().then(text => {console.log(text); })
        }
    }).catch(error => console.log(error));

    return false;
}

function cancelFestivalForm()
{
    formAction = "Create"; // pretvaranje u formu za dodavanje 
};

// reset forme za festival i ucitanje svih festivala i njihovo smestanje u tabelu
function refreshTable()
{
    document.getElementById("FestivalForm").reset();

    loadFestivals();
}

//edit festival:
// slanje GET ZAHTEVA, PREUZIMANJE OBJEKTA, ISPISVANJE VREDNOSTI U FORMU (UPDATE)
function editFestival()
{
   var editId = this.name;

   var url = host + port + festivaliEndpoint + editId.toString();
   console.log("Slanje zahteva: " + url);
   var headers = { };
   if(jwt_token)
   {
        headers.Authorization = "Bearer " + jwt_token;      
   }

   fetch(url, {headers: headers})
   .then((response) =>
   {
     if(response.status === 200)
     {
        console.log("Uspesna akcija!");
        response.json().then( data =>
            {
                document.getElementById("festivalNaziv").value = data.name;
                document.getElementById("festivalCenaKarte").value = data.ticketPrice;
                document.getElementById("festivalGodina").value = data.yearOfFirstEvent;
                document.getElementById("festivalPlace").value = data.placeId;
                editingId = data.id;
                formAction = "Update";           
            })
     }
     else{
         formAction = "Create";
         console.log("Greska prilikom preuzimanja festivala " + response.status);
         allert("Greska prilikom preuzimanja festivala!");
         response.text().then(text => {console.log(text); })
     }
   }).catch(error => console.log(error));
    
};

// brisanje festivala
function deleteFestival()
{
    var deleteId = this.name;
    var url = host + port + festivaliEndpoint + deleteId.toString();
    console.log("Url zahteva: " + url);

    var headers = { };
    if(jwt_token)
    {
        headers.Authorization = "Bearer " + jwt_token;
    }

    fetch(url, {method: "DELETE", headers: headers})
    .then((response) => 
    {
        if(response.status === 204)
        {
            console.log("Uspesna akcija!");
            refreshTable();
        }
        else
        {
            console("Greska prilikom: " + response.status);
            alert("Greska!");
            response.text().then(text => {console.log(text); })
        }
    }).catch(error => console.log(error));

};

// pretraga izmedju dva broja:
function  submitSearchFestivalForm()
{
    var start = document.getElementById("start").value;
    var end =  document.getElementById("end").value;

    if(validationSearchForm(start, end))
    {
        var url = host + port + searchEndpoint;
        var sendData = {"Start": start, "End": end};
        var headers = {'Content-Type': 'application/json'};
        if(jwt_token)
        {
            headers.Authorization = "Bearer " + jwt_token;
        }

    fetch(url, {method:"POST", headers:headers, body: JSON.stringify(sendData)})
    .then((response)=>
    {
        if(response.status === 200)
        {
            console.log("Uspesna akcija!");
            document.getElementById("searchFestivalForm").reset();
            response.json().then(setFestivals);          
        }
        else
        {
            console.log("Greska prilikom ucitavanja festivala kao rezultata pretrage: " + response.status);
            showError();
            response.text().then(text => {console.log(text); })
        }
    }).catch(error => console.log(error));
   
    }

    return false;
}

function validationSearchForm(start, end)
{
    if(start < 1950 || end > 2018)
    {
        alert("Godina mora biti u veca od 1950 i manja od 2018!");
        return false;
    }
    return true;
}



