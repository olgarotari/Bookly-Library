

async function loginUser(event) {
    event.preventDefault();

    const email = document.getElementById('inputEmail').value;
    const password = document.getElementById('inputPassword').value;

    fetch('/api/authentication/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        body: JSON.stringify({ Email: email, Password: password })
    }).then(function (res) {
        if (res.ok) {
            window.location.href = '/html/home.html';
        } else {
            debugger;
            
            alert(res);
        }
    });

   
}