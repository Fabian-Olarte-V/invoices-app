import { Routes } from '@angular/router';
import { InvoiceListView } from './views/invoice-list/invoice-list-view.component';
import { CreateInvoiceView } from './views/create-invoice/create-invoice-view.component';
import { HomeComponent } from './views/home/home';


export const routes: Routes = [
    { 
        path: '', 
        component: HomeComponent
    },
    { 
        path: 'create', 
        component: CreateInvoiceView
    },
    { 
        path: 'list', 
        component: InvoiceListView
    }
];
