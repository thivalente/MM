function findInArrayIndex(array, key, value)
{
    if (!array)
        return null;

    for (var i = 0, len = array.length; i < len; i++)
    {
        if (Array.isArray(key))
        {
            if (findInArrayValidateMultiple(array[i], key, value))
                return i;
        }
        else if (eval('array[' + i + '].' + key) === value)
            return i;
    }

    return null;
}

function findInArrayValidateMultiple(obj, key, value)
{
    var result = [];

    for (var property in obj)
    {
        for (var i = 0; i <= key.length - 1; i++)
        {
            if (obj.hasOwnProperty(property) && property === key[i])
            {
                result.push((eval('obj.' + key[i]) === value[i]));
            }
        }
    }

    if (result.length !== key.length)
        return false;

    for (var i = 0; i <= result.length - 1; i++)
    {
        if (!result[i])
            return false;
    }

    return true;
}

function isEmpty(obj)
{
    return obj === null || obj === undefined || obj === '' || obj.toString().trim() === '' || isObjectEmpty(obj);
}

function isObjectEmpty(obj)
{
    if (typeof obj !== 'object')
        return false;

    for (var key in obj)
    {
        if (obj.hasOwnProperty(key))
            return false;
    }
    return true;
}

function number_format(number, decimals, dec_point, thousands_sep)
{
    // *     example: number_format(1234.56, 2, ',', ' ');
    // *     return: '1 234,56'
    number = (number + '').replace(',', '').replace(' ', '');
    
    var n = !isFinite(+number) ? 0 : + number,
            prec = !isFinite(+decimals) ? 0 : Math.abs(decimals),
            sep = (typeof thousands_sep === 'undefined') ? ',' : thousands_sep,
            dec = (typeof dec_point === 'undefined') ? '.' : dec_point,
            s = '',
            toFixedFix = function (n, prec) {
                var k = Math.pow(10, prec);
                return '' + Math.round(n * k) / k;
            };

    // Fix for IE parseFloat(0.55).toFixed(0) = 0;
    s = (prec ? toFixedFix(n, prec) : '' + Math.round(n)).split('.');

    if (s[0].length > 3)
    {
        s[0] = s[0].replace(/\B(?=(?:\d{3})+(?!\d))/g, sep);
    }

    if ((s[1] || '').length < prec)
    {
        s[1] = s[1] || '';
        s[1] += new Array(prec - s[1].length + 1).join('0');
    }
    
    return s.join(dec);
}

function obterAnoMes(data) // Formato dd/MM/yyyy
{
    var ano = data.substring(6);
    var mes = data.substring(3).padStart(2, '0');

    return parseInt(ano+mes);
}

function obterDataFormatada_ddMMyyyy(javascriptDate) // Formato dd/MM/yyyy
{
    return javascriptDate.toJSON().slice(0,10).split('-').reverse().join('/');
}

function obterDataJavascript(formattedDate) // Formato dd/MM/yyyy
{
    if (isEmpty(formattedDate))
        return null;

    var year = formattedDate.substr(6, 4);
    var month = formattedDate.substr(3, 2);
    var day = formattedDate.substr(0, 2);

    if (formattedDate.length > 10) // Obter o horário
    {
        var time = formattedDate.substr(11).trim().split(':');
        var hour = 0;
        var min = 0;
        var sec = 0;

        if (time.length >= 1)
        {
            hour = time[0];

            if (time.length >= 2)
            {
                min = time[1];

                if (time.length >= 3)
                    sec = time[2];
            }
        }

        return new Date(year, month, day, hour, min, sec);
    }
    else // Sem horário
        return new Date(year, month, day, 0, 0, 0);
}

function obterNomeMesAno(anomes)
{
    var ano = anomes.toString().substring(0, 4);
    var mes = anomes.toString().substring(4);

    switch (mes)
    {
        case '1':
        case '01':
            return 'Janeiro/' + ano;
        case '2':
        case '02':
            return 'Fevereiro/' + ano;
        case '3':
        case '03':
            return 'Março/' + ano;
        case '4':
        case '04':
            return 'Abril/' + ano;
        case '5':
        case '05':
            return 'Maio/' + ano;
        case '6':
        case '06':
            return 'Junho/' + ano;
        case '7':
        case '07':
            return 'Julho/' + ano;
        case '8':
        case '08':
            return 'Agosto/' + ano;
        case '9':
        case '09':
            return 'Setembro/' + ano;
        case '10':
            return 'Outubro/' + ano;
        case '11':
            return 'Novembro/' + ano;
        case '12':
            return 'Dezembro/' + ano;
        default:
            return '';
    }
}

function obterNomeMesAnoReduzido(anomes)
{
    var ano = anomes.toString().substring(0, 4);
    var mes = anomes.toString().substring(4);

    switch (mes)
    {
        case '1':
        case '01':
            return 'Jan/' + ano;
        case '2':
        case '02':
            return 'Fev/' + ano;
        case '3':
        case '03':
            return 'Mar/' + ano;
        case '4':
        case '04':
            return 'Abr/' + ano;
        case '5':
        case '05':
            return 'Mai/' + ano;
        case '6':
        case '06':
            return 'Jun/' + ano;
        case '7':
        case '07':
            return 'Jul/' + ano;
        case '8':
        case '08':
            return 'Ago/' + ano;
        case '9':
        case '09':
            return 'Set/' + ano;
        case '10':
            return 'Out/' + ano;
        case '11':
            return 'Nov/' + ano;
        case '12':
            return 'Dez/' + ano;
        default:
            return '';
    }
}

function obterTotalMesesEntreDuasDatas(d1, d2)
{
    var months;
    months = (d2.getFullYear() - d1.getFullYear()) * 12;
    months -= d1.getMonth();
    months += d2.getMonth();
    return months <= 0 ? 0 : months;
}

function sortByKey(array, key, asc)
{
    return array.sort(function(a, b)
    {
        var x = a[key]; var y = b[key];

        if (asc)
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
        else
            return ((x < y) ? ((x > y) ? 1 : 0) : -1);
    });
}

function sortByKey_Date(array, key, asc)
{
    return array.sort(function(a, b)
    {
        var x = obterDataJavascript(a[key]);
        var y = obterDataJavascript(b[key]);

        if (asc)
            return ((x < y) ? -1 : ((x > y) ? 1 : 0));
        else
            return ((x < y) ? ((x > y) ? 1 : 0) : -1);
    });
}