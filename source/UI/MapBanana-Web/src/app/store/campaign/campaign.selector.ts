import { IAppState } from 'src/app/app.state';
import { ICampaign } from './campaign.model';

import { createSelector } from '@ngrx/store';
 
export const selectCampaign = createSelector(
    (state: IAppState) => state.campaign,
    (campaign: ICampaign) => campaign
);

export const selectActiveMapId = createSelector(
    (state: IAppState) => state.campaign,
    (campaign: ICampaign) => campaign.activeMapId
);