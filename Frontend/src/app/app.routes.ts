import { Routes } from '@angular/router';


export const routes: Routes = [
    {
        path: '',
        redirectTo: 'invoices',
        pathMatch: 'full'
    },
    {
        path: 'invoices',
        loadChildren: () => import('./modules/invoices/invoices-module').then(m => m.InvoiceModule)
    },
    {
        path: '**',
        redirectTo: 'invoices'
    }
];
