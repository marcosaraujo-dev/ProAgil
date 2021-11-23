import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventosComponent } from './eventos/eventos.component';
import { EventoEditComponent } from './eventos/eventoEdit/eventoEdit.component';
import { PalestrantesComponent } from './palestrantes/palestrantes/palestrantes.component';
import { ContatosComponent } from './contatos/contatos/contatos.component';
import { DashboardsComponent } from './dashboards/dashboards/dashboards.component';
import { UserComponent } from './user/user.component';
import { LoginComponent } from './user/login/login.component';
import { RegistrationComponent } from './user/registration/registration.component';
import { AuthGuard } from './auth/auth.guard';


const routes: Routes = [
  {path: 'user', component: UserComponent,
  children:[
    {path: 'login', component: LoginComponent},
    {path: 'registration', component: RegistrationComponent}
  ]
  },
  {path: 'eventos', component: EventosComponent, canActivate: [AuthGuard]},
  {path: 'evento/:id/edit', component: EventoEditComponent, canActivate: [AuthGuard]},
  {path: 'palestrantes', component: PalestrantesComponent, canActivate: [AuthGuard]},
  {path: 'contatos', component: ContatosComponent, canActivate: [AuthGuard]},
  {path: 'dashboards', component: DashboardsComponent},
  {path: '', redirectTo: 'dashboards', pathMatch: 'full'},
  {path: '**', redirectTo: 'dashboards', pathMatch: 'full'},

];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
