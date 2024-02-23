function validateForm(event) {
    event.preventDefault();

    var username = document.getElementById('username').value;
    var password = document.getElementById('password').value;
    var rememberMe = document.getElementById('remember').checked;

    //session storage - hostname
    sessionStorage.setItem("username", username);
    window.location.href = "home.html";

    if (username === 'abcd' && password === 'abcd') {
        window.location.href = 'home.html';
    } else {
        document.getElementById('error-message').innerText = 'Invalid credentials. Please try again.';
    }
}

function logOut(){
    alert("Are you sure you want to log out?");
    window.location.href = "main.html";
}
