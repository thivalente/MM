export interface Usuario
{
    id: string;
    nome: string;
    cpf: string;
    email: string;
    senha: string;
    taxa_acima_cdi: number;
    aceitou_termos: boolean;
    data_aceitou_termos: Date;
    is_admin: boolean;
    ativo: boolean;

    primeiro_nome: string;
    pagina_inicial: string;
}
