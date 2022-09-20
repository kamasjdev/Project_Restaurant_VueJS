export default function mapExceptionToMessage(exception) {
    const somethingBadHappen = 'Coś poszło nie tak';
    if (exception === null || exception === undefined) {
        return somethingBadHappen;
    }

    if (exception.response === null || exception.response === undefined) {
        return somethingBadHappen;
    }

    if (exception.response.data === null || exception.response.data === undefined) {
        return getMessageFromStatusCode(exception.response.status);
    }

    if (exception.response.data.code === null || exception.response.data.code === undefined) {
        return getMessageFromStatusCode(exception.response.status);
    }

    return getMessageFromCode(exception.response.data.code);
}

function getMessageFromStatusCode(status) {
    const genericMessage = 'Wystąpił błąd';
    if (status === 404) {
        return 'Nie znaleziono';
    } else {
        return genericMessage;
    }
}

function getMessageFromCode(code) {
    switch(code) {
        case 'product_name_too_short': {
            return 'Nazwa produktu powinna zawierać przynajmniej 3 znaki';
        }
        case 'cannot_delete_product_ordered': {
            return 'Nie można usunąć produktu ponieważ jest już zamówiony';
        }
        case 'product_not_found': {
            return 'Produkt nie został znaleziony';
        }
        case 'addition_name_too_short': {
            return 'Nazwa dodatku powinna zawierać przynajmniej 3 znaki';
        }
        case 'cannot_delete_addition_ordered': {
            return 'Nie można usunąć dodatku ponieważ jest już zamówiony';
        }
        case 'addition_not_found': {
            return 'Dodatek nie został znaleziony';
        }
        case 'product_sale_not_found': {
            return 'Nie znaleziono pozycji zamówienia';
        }
        case 'order_not_found': {
            return 'Nie znaleziono zamówienia';
        }
        case 'invalid_credentials': {
            return 'Niepoprawne dane logowania';
        }
        default: {
            return 'Wystąpił błąd';
        }
    }
}