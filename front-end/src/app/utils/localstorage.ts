import { Usuario } from '../_models/usuario';
import { Taxas } from '../_models/taxas';

export class LocalStorageUtils
{
    public limparDadosLocaisUsuario()
    {
        localStorage.removeItem('mm.taxas');
        localStorage.removeItem('mm.token');
        localStorage.removeItem('mm.user');
    }

    public obterTaxasAtualizadas(): Taxas
    {
        return JSON.parse(localStorage.getItem('mm.taxas'));
    }


    public obterTokenUsuario(): string
    {
        return localStorage.getItem('mm.token');
    }

    public obterUsuario() : Usuario
    {
        return JSON.parse(localStorage.getItem('mm.user'));
    }

    public salvarDadosLocaisUsuario(response: any)
    {
        this.salvarTaxas(response.taxas);
        this.salvarTokenUsuario(response.accessToken);
        this.salvarUsuario(response.userToken);
    }

    public salvarTaxas(taxas: string)
    {
        localStorage.setItem('mm.taxas', JSON.stringify(taxas));
    }

    public salvarTokenUsuario(token: string)
    {
        localStorage.setItem('mm.token', token);
    }

    public salvarUsuario(user: string)
    {
        localStorage.setItem('mm.user', JSON.stringify(user));
    }
}