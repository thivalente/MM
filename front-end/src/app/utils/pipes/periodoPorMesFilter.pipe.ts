import { Pipe, PipeTransform } from '@angular/core';

@Pipe({ name: 'periodoPorMesFilter', pure: false })

export class PeriodoPorMesFilterPipe implements PipeTransform
{
    transform(items: any[], filter: string): any
    {
        if (!items || !filter)
        {
            return items;
        }

        return items.filter(item => item.periodo.toString().indexOf(filter) !== -1);
    }
}