import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';

import { GlComponent } from './gl/gl.component';
import { PlayerComponent } from './player/player.component';
import { StoreModule } from '@ngrx/store';
import { campaignsReducer } from './store/campaigns/campaigns.reducer';
import { CampaignComponent } from './campaign/campaign.component';
import { mapsReducer } from './store/maps/maps.reducer';
import { campaignReducer } from './store/campaign/campaign.reducer';
import { mapReducer } from './store/map/map.reducer';

@NgModule({
  declarations: [
    AppComponent,
    GlComponent,
    PlayerComponent,
    CampaignComponent
  ],
  imports: [
    BrowserModule,
    HttpClientModule,
    AppRoutingModule,
    StoreModule.forRoot({
      campaigns: campaignsReducer,
      campaign: campaignReducer,
      maps: mapsReducer,
      map: mapReducer
    })
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
