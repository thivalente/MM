import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'periodoHistoricoFilter', pure: false })

export class PeriodoHistoricoFilterPipe implements PipeTransform
{
    transform(items: any[]): any
    {
        if (!items)
            return items;

        var mes = (new Date().getMonth() + 1).toString().padStart(2, '0');

        return items.filter(item =>
            item.data.indexOf('/' + mes + '/') > -1 // Se for o mês corrente
            || !item.rendimento // ou quando for aplicação ou resgate
        );
    }
}