function verifyRegistration() {
    const firstName = document.getElementById('firstName').value;
    const lastName = document.getElementById('lastName').value;
    const email = document.getElementById('email').value;
    const phoneNumber = document.getElementById('phoneNumber').value;
    const gender = document.querySelector('input[name="gender"]:checked');

    const password = document.getElementById('password').value;
    const rePassword = document.getElementById('rePassword').value;

    if (firstName === '' || lastName === '' || email === '' || phoneNumber === '' || !gender || password === '' || rePassword === '') {
        alert("Fill all fields");
    } else {
        if (password === rePassword) {
            window.location.href = 'login.html';
        } else {
            alert("Passwords do not match. Try again.");
            window.location.href = 'register.html';
        }
    }
}