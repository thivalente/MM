import { Injectable } from '@angular/core';

import { environment } from 'src/environments/environment';

@Injectable()
export class SettingsService
{
    constructor() { }

    getApiUrl() { return environment.apiUrlv1;; }

    getCaptchaData() { return { sitekey: '6LdNuj8UAAAAACATXK6oD6DvOUXnIdSC8VPBh7Sa', secretkey: '6LdNuj8UAAAAAIxW19wu6pDgFHgn6xIvnIIggU-G', theme: 'light' }; }

    getLocale() { return 'pt-BR'; }
}
