export interface Movimentacao
{
    id: string;
    usuario_id: string;
    valor: number;
    data_criacao: string;
    tipo: string;
    entrada: boolean;
    ativo: boolean;
}
