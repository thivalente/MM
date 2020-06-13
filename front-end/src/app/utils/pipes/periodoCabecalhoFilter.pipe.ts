import { Pipe, PipeTransform } from '@angular/core';

declare const obterAnoMes: any;
declare const sortByKey_Date: any;

@Pipe({ name: 'periodoCabecalhoFilter', pure: false })
export class PeriodoCabecalhoFilterPipe implements PipeTransform
{
    transform(items: any[]): any
    {
        if (!items)
            return items;

        var result = [];
        items.forEach(m =>
        {
            var periodo = obterAnoMes(m.data);
    
            if (result.filter(r => r.data === m.data).length === 0) // Se não encontrar, insere cabeçalho
                result.push({ cabecalho: true, entrada: false, rendimento: false, valor: 0, data: m.data, periodo: periodo });
    
            result.push(m);
        });

        //return sortByKey_Date(result.map(m => Object.assign({}, m)), 'data', true);
        return result;
    }
}