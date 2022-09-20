export function login(data) {
    data.expiry = new Date().getTime() + 60*60*1000;
    window.localStorage.setItem('user-data', JSON.stringify(data));
}

export function logout() {
    window.localStorage.removeItem('user-data');
}

export function isLogged() {
    const tokenData = JSON.parse(window.localStorage.getItem('user-data'));

    if (tokenData === null || tokenData === undefined) {
        return false;
    }

    const tokenExpiresDate = new Date(tokenData.expiry);
    const currentDate = new Date();
    
    if (tokenExpiresDate < currentDate) {
        window.localStorage.removeItem('user-data');
        return false;
    }

    return true;
}