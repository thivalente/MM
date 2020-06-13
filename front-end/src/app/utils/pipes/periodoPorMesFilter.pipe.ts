import { Pipe, PipeTransform } from '@angular/core';

declare const sortByKey_Date: any;

@Pipe({ name: 'periodoPorMesFilter', pure: false })
export class PeriodoPorMesFilterPipe implements PipeTransform
{
    transform(items: any[], filter: string): any
    {
        if (!items || !filter)
        {
            return items;
        }

        var result = items.filter(item => item.periodo.toString().indexOf(filter) !== -1);

        //return sortByKey_Date(result.map(m => Object.assign({}, m)), 'data', true);
        return result;
    }
}