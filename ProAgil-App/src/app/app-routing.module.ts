import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventosComponent } from './eventos/eventos.component';
import { PalestrantesComponent } from './palestrantes/palestrantes/palestrantes.component';
import { ContatosComponent } from './contatos/contatos/contatos.component';
import { DashboardsComponent } from './dashboards/dashboards/dashboards.component';

const routes: Routes = [
  {path: 'eventos', component: EventosComponent},
  {path: 'palestrantes', component: PalestrantesComponent},
  {path: 'contatos', component: ContatosComponent},
  {path: 'dashboards', component: DashboardsComponent},
  {path: '', redirectTo: 'dashboards', pathMatch: 'full'},
  {path: '**', redirectTo: 'dashboards', pathMatch: 'full'},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
