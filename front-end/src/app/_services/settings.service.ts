import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';

@Injectable()
export class SettingsService
{
    constructor() { }

    getApiUrl() { return environment.apiUrlv1;; }

    getLocale() { return 'pt-BR'; }
}
