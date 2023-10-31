function PasswordMatch() {
    var password = document.getElementById("Password");
    var confirmPassword = document.getElementById("ConfirmPassword");
    var message = document.getElementById("password-match-message");

    if (password.value !== confirmPassword.value) {
        message.textContent = "Les mots de passe ne sont pas identiques.";
    }
    else {
        message.textContent = "";
    }
}

function EmailMatch() {
    var Email = document.getElementById("Email");
    var confirmEmail = document.getElementById("confirmemail");
    var message = document.getElementById("Email-match-message");

    if (Email.value !== confirmEmail.value) {
        message.textContent = "L'adresse e-mail ne concorde pas.";
    }
    else {
        message.textContent = "";
    }
}

function Emailvalid() {
    const emailRegex = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    var Email = document.getElementById("Email");
    var message = document.getElementById("validEmail-match-message");

    if (!emailRegex.test(Email.value)) {
        message.innerHTML = ' <P>L\'adresse e-mail incorrecte.</P>';
        console.log(Email.value)
    }
    else {
        message.textContent = "";
    }

}