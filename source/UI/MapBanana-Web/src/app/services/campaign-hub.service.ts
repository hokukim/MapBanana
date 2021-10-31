import { Injectable } from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@microsoft/signalr';
import { stringify } from 'querystring';
import { BananaApiService } from './banana-api.service';

@Injectable({
  providedIn: 'root'
})
export class CampaignHubService {

  private hubConnection: HubConnection;
  private campaignHubUrl: string = 'https://localhost:6001/campaignHub';

  constructor(private bananaApiService: BananaApiService) {
    this.hubConnection = new HubConnectionBuilder()
      .withUrl(this.campaignHubUrl)
      .withAutomaticReconnect()
      .build();
  }

  public connect(campaignId: string): void {
    this.hubConnection
      .start()
      .then(() => {
        console.log(`Connecting to campaign: ${campaignId}`);

        this.hubConnection.send('Join', campaignId)
          .then(() => {
            console.log(`Successfully connected to campaign: ${campaignId} `)
          });
      })
      .catch(error => console.log(`Error connecting to campaign ${campaignId}: ${error}`));

    this.hubConnection.on('MapAdded', (campaignId: string) => 
    {
      // Get maps.
      this.bananaApiService.getMaps(campaignId);

      // Write to store.
      //
    });
    
    this.hubConnection.on('MapActive', () => 
    {
      console.log('Campaign message: Map activated')

      // Get active map.
      //

      // Write to store.
      //
    });
  }
}
