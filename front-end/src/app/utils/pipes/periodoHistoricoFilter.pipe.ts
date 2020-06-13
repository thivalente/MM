import { Pipe, PipeTransform } from '@angular/core';

declare const sortByKey_Date: any;

@Pipe({ name: 'periodoHistoricoFilter', pure: false })
export class PeriodoHistoricoFilterPipe implements PipeTransform
{
    transform(items: any[]): any
    {
        if (!items)
            return items;

        var mes = (new Date().getMonth() + 1).toString().padStart(2, '0');

        var result = items.filter(item =>
            item.data.indexOf('/' + mes + '/') > -1 // Se for o mês corrente
            || !item.rendimento // ou quando for aplicação ou resgate
        );

        //return sortByKey_Date(result.map(m => Object.assign({}, m)), 'data', true);
        return result;
    }
}