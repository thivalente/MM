import { Usuario } from '../_models/usuario';

export class LocalStorageUtils
{
    public limparDadosLocaisUsuario()
    {
        localStorage.removeItem('mm.token');
        localStorage.removeItem('mm.user');
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
        this.salvarTokenUsuario(response.accessToken);
        this.salvarUsuario(response.userToken);
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