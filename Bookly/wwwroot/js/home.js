

async function loginUser(event) {
    event.preventDefault();

    const email = document.getElementById('inputEmail').value;
    const password = document.getElementById('inputPassword').value;

    fetch('/api/authentication/login', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
        body: JSON.stringify({ Email: email, Password: password })
    }).then(function (res) {
        if (res.ok) {
            window.location.href = '/Account/Profile';
        } else {
            /*debugger;*/
            
            alert(res);
        }
    });

   
}

async function registerUser(event) {
    event.preventDefault();

    const firstName = document.getElementById('inputFirstName').value;
    const lastName = document.getElementById('inputLastName') ? document.getElementById('inputLastName').value : '';
    const fullName = `${firstName} ${lastName}`.trim(); 

    const email = document.getElementById('inputEmail').value;
    const password = document.getElementById('inputPassword').value;
    const confirmPassword = document.getElementById('inputPasswordConfirm').value; 

    fetch('/api/authentication/register', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json'
        },
        credentials: 'include',
        body: JSON.stringify({
            FullName: fullName,
            Email: email,
            Password: password,
            ConfirmPassword: confirmPassword
        })
    }).then(async function (res) {
        if (res.ok) {
            window.location.href = '/';
        } else {
            const errorText = await res.text();
            alert(errorText || 'A apărut o eroare la înregistrare!');
        }
    });
}

async function searchBooks() {
    const searchInput = document.getElementById('searchInput');
    if (!searchInput) return;

    const term = searchInput.value.trim();
    if (!term) return;

    try {
        // Trimitem cererea la backend către LiveSearch
        const response = await fetch(`/api/Book/live-search?term=${encodeURIComponent(term)}`);
        if (!response.ok) return;

        const books = await response.json();

        // funcția care desenează cardurile pe ecran
        displayBooks(books);

    } catch (error) {
        console.error("Eroare la căutare:", error);
    }
}

// Conectăm funcția la evenimentul de input (când tastezi în search)
document.getElementById('searchInput')?.addEventListener('input', searchBooks);


